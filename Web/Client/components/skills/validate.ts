import * as v from "valibot";

export const skillLanguageForm = v.object({
  locale: v.string(),
  name: v.string(),
  description: v.string(),
});

export type SkillLanguageFormObj = v.Input<typeof skillLanguageForm>;

export const skillForm = v.object({
  name: v.string(),
  description: v.string(),
  category: v.number([v.integer()]),
  summon: v.number([v.integer()]),
  ratingPhysical: v.number([v.integer(), v.minValue(0), v.maxValue(10)]),
  ratingMagical: v.number([v.integer(), v.minValue(0), v.maxValue(10)]),
  costBuy: v.number([v.integer()]),
  costUpgrade: v.number([v.integer()]),
  costMaster: v.number([v.integer()]),
  iconFile: v.optional(v.instance(File)),
  iconUrl: v.optional(v.string()),
  previewFile: v.optional(v.instance(File)),
  previewImageUrl: v.optional(v.string()),
});

export type SkillFormObj = v.Input<typeof skillForm>;
