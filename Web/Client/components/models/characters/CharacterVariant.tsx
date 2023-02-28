export interface ICharacter {
    id: number;
    characterId: number;

    description: string;
    defaultVariant: boolean;
    age: number;

    additionalFields: ICharacterVariantField[];

    previewImageUrl?: string;
}