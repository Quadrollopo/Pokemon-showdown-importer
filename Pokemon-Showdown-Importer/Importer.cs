using System;
using System.IO;

namespace PokemonShowdownImporter {
	
	
	
	public static class Importer {
		private static string[] tables = {
			"ABCD",
			"ABDC",
			"ACBD",
			"ACDB",
			"ADBC",
			"ADCB",
			"BACD",
			"BADC",
			"BCAD",
			"BCDA",
			"BDAC",
			"BDCA",
			"CABD",
			"CADB",
			"CBAD",
			"CBDA",
			"CDAB",
			"CDBA",
			"DABC",
			"DACB",
			"DBAC",
			"DBCA",
			"DCAB",
			"DCBA"
		};
		private class Decrypter {
			private uint seed;
			public Decrypter(uint seed) {
				this.seed = seed;
			}

			public ushort Next() {
				uint res = 0x41C64E6D * seed + 0x6073;
				seed = res;
				return (ushort)(res >> 16);
			}

		}
		
		public static string ReadSaveFile(string path) {
			byte[] blocks = File.ReadAllBytes(path);
			string outputText = "";
			BinaryReader binaryReader = new BinaryReader(File.Open(path, FileMode.Open, FileAccess.Read));
			//int p = binaryReader.Read(b, 1, 9);
			int partyNum = blocks[0x94];
			for (int n = 0; n < partyNum; n++) {
				int pokemonOffset = 0x98 + n * 236;
				uint pv = BitConverter.ToUInt32(blocks, pokemonOffset);
				ushort checksum = BitConverter.ToUInt16(blocks, pokemonOffset + 6);
				Decrypter decrypter = new Decrypter(checksum);
				//Decrypt the pokemon information
				for (int off = pokemonOffset + 8; off < pokemonOffset + 8 + 128; off+=2) {
					ushort word = BitConverter.ToUInt16(blocks, off);
					word = (ushort)(word ^ decrypter.Next());
					byte[] decryptedWord = BitConverter.GetBytes(word);
					blocks[off] = decryptedWord[0];
					blocks[off+1] = decryptedWord[1];
				}
				// Battle stats info
				decrypter = new Decrypter(pv);
				for (int off = pokemonOffset + 0x88; off < pokemonOffset + 0x88 + 100; off+=2) {
					ushort word = BitConverter.ToUInt16(blocks, off);
					word = (ushort)(word ^ decrypter.Next());
					byte[] decryptedWord = BitConverter.GetBytes(word);
					blocks[off] = decryptedWord[0];
					blocks[off+1] = decryptedWord[1];
				}

				byte level = blocks[pokemonOffset + 0x8C];
				int tableOffset;
				string tableOrder = tables[((pv & 0x3E000) >> 0xD) % 24];

				// region A Table
				tableOffset = tableOrder.IndexOf('A') * 32 + pokemonOffset + 8;
				ushort pkmn_id = BitConverter.ToUInt16(blocks, tableOffset);
				ushort item = BitConverter.ToUInt16(blocks, tableOffset + 2);
				byte ability = blocks[tableOffset + 13];
				
				//EVs
				byte hp_ev = blocks[tableOffset + 16];
				byte atk_ev = blocks[tableOffset + 17];
				byte def_ev = blocks[tableOffset + 18];
				byte spe_ev = blocks[tableOffset + 19];
				byte spA_ev = blocks[tableOffset + 20];
				byte spD_ev = blocks[tableOffset + 21];

				//endregion
				
				// region B Table
				tableOffset = tableOrder.IndexOf('B') * 32 + pokemonOffset + 8;
				
				//Moves
				ushort mv1 = BitConverter.ToUInt16(blocks, tableOffset);
				ushort mv2 = BitConverter.ToUInt16(blocks, tableOffset + 2);
				ushort mv3 = BitConverter.ToUInt16(blocks, tableOffset + 4);
				ushort mv4 = BitConverter.ToUInt16(blocks, tableOffset + 6);
				
				//IVs
				uint ivs_chunk = BitConverter.ToUInt32(blocks, tableOffset + 16);
				byte hp_iv = (byte)(ivs_chunk & 0x1F);
				byte atk_iv = (byte)(ivs_chunk>>5 & 0x1F);
				byte def_iv = (byte)(ivs_chunk>>10 & 0x1F);
				byte spe_iv = (byte)(ivs_chunk>>15 & 0x1F);
				byte spA_iv = (byte)(ivs_chunk>>20 & 0x1F);
				byte spD_iv = (byte)(ivs_chunk>>25 & 0x1F);

				bool isNicknamed = blocks[tableOffset + 19] >> 7 == 1;
				//endregion
				
				// region C Table
				tableOffset = tableOrder.IndexOf('C') * 32 + pokemonOffset + 8;
				string nickname = "";
				if (isNicknamed) {
					for (int i = 0; i < 22; i+=2) {
						ushort c = BitConverter.ToUInt16(blocks, tableOffset + i);
						if(c == 65535)
							break;
						if (c < 325)
							nickname += (char)(c - 234);
						else
							nickname += (char)(c - 228);
					}
				}
				
				//endregion
				if (isNicknamed)
					outputText += $"{nickname} ({Data.pokedex[pkmn_id]}) ";
				else
					outputText += Data.pokedex[pkmn_id];

				if (item != 0)
					outputText += "@ " + Data.items[item];
				outputText += '\n';
				outputText += $"Level: {level}\n";
				outputText += $"Ability: {Data.abilities[ability]}\n";
				outputText += $"Nature: {Data.natures[(int)(pv % 25)]}\n";
				outputText += $"EVs: {hp_ev} HP / {atk_ev} Atk / {def_ev} Def / {spA_ev} SpA / {spD_ev} SpD / {spe_ev} Spe\n";
				outputText += $"IVs: {hp_iv} HP / {atk_iv} Atk / {def_iv} Def / {spA_iv} SpA / {spD_iv} SpD / {spe_iv} Spe\n";
				if(mv1 != 0)
					outputText += $"-{Data.moves[mv1]}\n";
				if(mv2 != 0)
					outputText += $"-{Data.moves[mv2]}\n";
				if(mv3 != 0)
					outputText += $"-{Data.moves[mv3]}\n";
				if(mv4 != 0)
					outputText += $"-{Data.moves[mv4]}\n";
				outputText += "\n";

			}

			return outputText;
		}
	}
}