import { ICharacterVariantField } from "./CharacterVariantField";

export interface ICharacterVariant {
  id: number;
  characterId: number;

  description: string;
  defaultVariant: boolean;
  age: number;

  fromYear: number;
  toYear: number;

  additionalFields: ICharacterVariantField[];

  previewImageUrl?: string;
}
