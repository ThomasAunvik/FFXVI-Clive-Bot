export interface ICharacter {
    id: number;
    name: string;
    defaultVariant: ICharacterVariant;
    variants: ICharacterVariant[];
    notes: ICharacterNote[];
}