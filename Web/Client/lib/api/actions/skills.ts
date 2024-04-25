"use server";

import type { SkillLanguageFormObj } from "@/components/skills/validate";
import {
	createSkill,
	deleteSkillLanguage,
	setSkillLanguage,
	updateSkill,
} from "@/lib/api/skills";
import type { ISkill } from "@/lib/models/skill/SkillModel";
import { revalidatePath } from "next/cache";

export const actionCreateSkill = async (skill: ISkill) => {
	const newSkill = await createSkill(skill);

	revalidatePath("/dashboard/skills");
	revalidatePath("/skills");

	return newSkill;
};

export const actionUpdateSkill = async (skillId: string, skill: ISkill) => {
	const newSkill = await updateSkill(skillId, skill);

	revalidatePath("/dashboard/skills");
	revalidatePath(`/dashboard/skills/${skillId}`);
	revalidatePath("/skills");

	return newSkill;
};

export const actionSetSkillLanguage = async (
	skillId: string,
	locale: string,
	data: SkillLanguageFormObj,
) => {
	await setSkillLanguage(skillId, locale, data);

	revalidatePath(`/dashboard/skills/${locale}`);
	revalidatePath("/skills");
};

export const actionDeleteSkillLanguage = async (
	skillId: string,
	locale: string,
) => {
	await deleteSkillLanguage(skillId, locale);

	revalidatePath(`/dashboard/skills/${locale}`);
	revalidatePath("/skills");
};
