import { ISkillLanguage } from "./SkillLanguageModel";

export enum SkillCategory {
  None,
  Defensive,
  Offensive,
}

export enum SkillSummon {
  None,
  Ifrit,
  Pheonix,
  Garuda,
  Shiva,
  Titan,
  Ramuh,
  Odin,
  Bahamut,
}

export interface ISkill {
  id: string;
  name: string;
  description: string;
  localized: ISkillLanguage[];
  category: SkillCategory;
  summon: SkillSummon;
  ratingPhysical: number;
  ratingMagical: number;
  masterizationPoints: number;

  iconUrl?: string;
  previewImageUrl?: string;

  masteredVersion?: ISkill;
  previousVersion?: ISkill;
}
