import { ICharacterNote } from "./CharacterNote";
import { ICharacterVariant } from "./CharacterVariant";

export interface ICharacter {
  id: number;
  name: string;
  defaultVariant?: ICharacterVariant;
  variants?: ICharacterVariant[];
  notes?: ICharacterNote[];
}
