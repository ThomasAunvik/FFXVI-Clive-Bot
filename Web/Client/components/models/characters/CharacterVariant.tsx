import { ICharacterVariantField } from "./CharacterVariantField";

export interface ICharacterVariant {
    id: number;
    characterId: number;

    description: string;
    defaultVariant: boolean;
    age: number;

    additionalFields: ICharacterVariantField[];

    previewImageUrl?: string;
}