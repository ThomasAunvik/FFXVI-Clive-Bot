import { apiGET } from "@/lib/api/fetch";
import type { ISkill } from "@/lib/models/skill/SkillModel";
import "server-only";

export const getSkills = async () => {
  return await apiGET<ISkill[]>("/api/skill");
};
