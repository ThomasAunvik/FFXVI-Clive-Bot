export interface IModeratorPermissions {
    id: number;
    manageModerators: boolean;
    allPermissions: boolean;

    manageSkills: boolean;
    manageSkillInfo: boolean;
    manageSkillTranslations: boolean;

    manageCharacters: boolean;

    manageCharacterInfo: boolean;
    manageCharacterNotes: boolean;
}