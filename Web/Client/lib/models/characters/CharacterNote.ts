export interface ICharacterNote {
  id: number;
  characterId: number;
  noteName: string;
  noteDescription: string;
  locale: string;

  previewImageUrl?: string;
}
