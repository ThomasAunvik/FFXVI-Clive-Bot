import type { ISkillLanguage } from "@/lib/models/skill/SkillLanguageModel";

export enum SkillCategory {
	None = 0,
	Attack = 1,
	Magic = 3,
	Other = 4,
	Jump = 5,
	Evade = 6,
}

export enum SkillSummon {
	General = 0,
	Ifrit = 1,
	Pheonix = 2,
	Garuda = 3,
	Shiva = 4,
	Titan = 5,
	Ramuh = 6,
	Odin = 7,
	Bahamut = 8,
	Leviathan = 9,
	Ultima = 10,
}

export const summonList = Object.values(SkillSummon)
	.filter((val) => Number.isInteger(val))
	.map((val) => val as number);

export const skillCategoryList: SkillCategory[] = Object.values(SkillCategory)
	.filter((val) => Number.isInteger(val))
	.map((val) => val as number);

export interface ISkill {
	id: number;
	name: string;
	description: string;
	localized: ISkillLanguage[];
	category: SkillCategory;
	summon: SkillSummon;
	ratingPhysical: number;
	ratingMagical: number;

	costBuy: number;
	costUpgrade: number;
	costMaster: number;

	iconUrl?: string;
	previewImageUrl?: string;
}
