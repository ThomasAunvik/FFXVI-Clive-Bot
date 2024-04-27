import type { SkillLanguageFormObj } from "@/components/skills/validate";
import { apiDELETE, apiGET, apiPOST, apiPUT } from "@/lib/api/fetch";
import type { ISkillLanguage } from "@/lib/models/skill/SkillLanguageModel";
import type { ISkill } from "@/lib/models/skill/SkillModel";
import "server-only";

export const getSkills = async () => {
	return await apiGET<ISkill[]>("/api/skill");
};

export const getSkill = async (skillId: string) => {
	return await apiGET<ISkill>(`/api/skill/${skillId}`);
};

export const createSkill = async (skill: ISkill) => {
	return await apiPOST<ISkill>("/api/skill", skill);
};

export const updateSkill = async (skillId: string, skill: ISkill) => {
	return await apiPUT<ISkill>(`/api/skill/${skillId}`, skill);
};

export const getSkillLanguages = async (skillId: string) => {
	return await apiGET<ISkillLanguage[]>(`/api/skill/${skillId}/languages`);
};

export const setSkillLanguage = async (
	skillId: string,
	locale: string,
	data: SkillLanguageFormObj,
) => {
	return await apiPOST<ISkillLanguage[]>(
		`/api/skill/${skillId}/languages/${locale}`,
		data,
	);
};

export const deleteSkillLanguage = async (skillId: string, locale: string) => {
	return await apiDELETE(`/api/skill/${skillId}/languages/${locale}`);
};
