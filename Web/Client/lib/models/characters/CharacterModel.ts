import type { ICharacterNote } from "./CharacterNote";
import type { ICharacterVariant } from "./CharacterVariant";

export interface ICharacter {
  id: number;
  name: string;
  defaultVariant?: ICharacterVariant;
  variants?: ICharacterVariant[];
  notes?: ICharacterNote[];
}
