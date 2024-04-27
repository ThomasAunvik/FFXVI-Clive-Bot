import * as v from "valibot";

export const permissionsForm = v.object({
  manageModerators: v.optional(v.boolean()),
  allPermissions: v.optional(v.boolean()),
  manageCharacterInfo: v.optional(v.boolean()),
  manageCharacterNotes: v.optional(v.boolean()),
  manageCharacters: v.optional(v.boolean()),
  manageSkillInfo: v.optional(v.boolean()),
  manageSkills: v.optional(v.boolean()),
  manageSkillTranslations: v.optional(v.boolean()),
});

export const moderatorFormSchema = v.required(
  v.object({
    name: v.string(),
    connectionSource: v.string(),
    connectionId: v.string(),
    permissions: v.optional(permissionsForm),
  }),
);

export type ModeratorFormData = v.Input<typeof moderatorFormSchema>;
