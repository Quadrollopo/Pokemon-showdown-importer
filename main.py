import json
from json import load
from sys import argv


class Decrypter:
	def __init__(self, seed):
		self.seed = seed

	def next(self):
		res = 0x41C64E6D * self.seed + 0x6073
		self.seed = res & 0xFFFFFFFF
		return (res >> 16) & 0xffff


if len(argv) == 1:
	save_name = "Pokemon.sav"
else:
	save_name = argv[1]

tables = {0: "ABCD",
		  1: "ABDC",
		  2: "ACBD",
		  3: "ACDB",
		  4: "ADBC",
		  5: "ADCB",
		  6: "BACD",
		  7: "BADC",
		  8: "BCAD",
		  9: "BCDA",
		  10: "BDAC",
		  11: "BDCA",
		  12: "CABD",
		  13: "CADB",
		  14: "CBAD",
		  15: "CBDA",
		  16: "CDAB",
		  17: "CDBA",
		  18: "DABC",
		  19: "DACB",
		  20: "DBAC",
		  21: "DBCA",
		  22: "DCAB",
		  23: "DCBA"}


with open("Data/pokedex.json", "r") as f:
	species = load(f)

with open("Data/items.json", "r") as f:
	items = load(f)

with open("Data/moves.json", "r") as f:
	moves = load(f)

with open("Data/abilities.json", "r") as f:
	abilities = load(f)

with open("Data/nature.json", "r") as f:
	natures = load(f)

with open(save_name, "rb") as f:
	f.seek(0x94)
	num_party = int.from_bytes(f.read(1), "little")
	for i in range(num_party):
		off = 0x98 + i * 236
		f.seek(off)
		pv = int.from_bytes(f.read(4), "little")
		f.seek(off + 0x06)
		checksum = int.from_bytes(f.read(2), "little")
		blocks = bytes()

		# Decrypt time
		dec = Decrypter(seed = checksum)
		while len(blocks) < 128:
			val = dec.next()
			word = f.read(2)
			word = int.from_bytes(word, 'little')
			res = word ^ val
			blocks += res.to_bytes(2, 'little')
		f.seek(off + 0x88)
		dec = Decrypter(seed = pv)
		while len(blocks) < 100 + 128:
			val = dec.next()
			word = f.read(2)
			word = int.from_bytes(word, 'little')
			res = word ^ val
			blocks += res.to_bytes(2, 'little')

		order = tables[((pv & 0x3E000) >> 0xD) % 24]

		# region C Table
		offset = order.find("B") * 32
		# Is nicknamed
		data = blocks[offset+19] >> 7
		nickname = ''
		if data:
			offset = order.find("C") * 32
			for b in range(offset, offset + 20, 2):
				c = int.from_bytes(blocks[b:b + 2], 'little')
				if c == 65535:
					break

				# Upper char
				if c < 325:
					nickname += chr(c - 234)
				else:
					nickname += chr(c - 228)
		# endregion

		# region A Table
		offset = order.find("A") * 32
		data = int.from_bytes(blocks[offset:offset+2], 'little')
		if nickname:
			name = f"{nickname} ({species[str(data)]})"
		else:
			name = {species[str(data)]}

		data = int.from_bytes(blocks[offset + 2:offset + 4], 'little')
		if data != 0:
			name += f" @ {items[str(data)]}"
		print(name)
		print(f"Level: {blocks[132]}")

		print(f"Ability: {abilities[str(blocks[offset+13])]}")

		print(f"{natures[str(pv % 25)]} Nature")

		# EVs
		print(f"EVs: {blocks[offset + 16]} HP / {blocks[offset + 17]} Atk / {blocks[offset + 18]} Def "
			  f"/ {blocks[offset + 19]} SpA / {blocks[offset + 20]} SpD / {blocks[offset + 21]} Spe")

		# endregion

		# region B Table
		offset = order.find("B") * 32

		# IVs
		data = int.from_bytes(blocks[offset+16:offset+20], 'little')
		print(f"IVs: {data & 0x1F} HP / {data>>5 & 0x1F} Atk / {data>>10 & 0x1F} Def "
			  f"/ {data>>15 & 0x1F} SpA / {data>>20 & 0x1F} SpD / {data>>25 & 0x1F} Spe")

		# Moves
		data = int.from_bytes(blocks[offset:offset+2], 'little')
		if data != 0:
			print(f"- {moves[str(data)]}")
		data = int.from_bytes(blocks[offset+2:offset+4], 'little')
		if data != 0:
			print(f"- {moves[str(data)]}")
		data = int.from_bytes(blocks[offset+4:offset+6], 'little')
		if data != 0:
			print(f"- {moves[str(data)]}")
		data = int.from_bytes(blocks[offset+6:offset+8], 'little')
		if data != 0:
			print(f"- {moves[str(data)]}")

		# endregion

		print()
