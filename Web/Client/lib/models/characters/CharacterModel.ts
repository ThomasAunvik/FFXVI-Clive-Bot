import type { ICharacterNote } from "@/lib/models/characters/CharacterNote";
import type { ICharacterVariant } from "@/lib/models/characters/CharacterVariant";

export interface ICharacter {
  id: number;
  name: string;
  defaultVariant?: ICharacterVariant;
  variants?: ICharacterVariant[];
  notes?: ICharacterNote[];
}
