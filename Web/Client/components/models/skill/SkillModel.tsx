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

export const summonList: SkillSummon[] =  Object.values(SkillSummon)
    .filter((val) => isNaN(Number(val)))
    .filter((val) => val != SkillSummon[SkillSummon.None])
    .map((val) => val as SkillSummon);

export const skillCategoryList: SkillCategory[] =  Object.values(SkillCategory)
    .filter((val) => isNaN(Number(val)))
    .map((val) => val as SkillCategory);

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
