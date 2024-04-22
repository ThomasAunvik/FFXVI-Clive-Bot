import * as v from "valibot";

export const characterForm = v.object({
  name: v.string(),
  previewFile: v.optional(v.instance(File)),
  previewImageUrl: v.optional(v.string()),
});

export type CharacterFormObj = v.Input<typeof characterForm>;

export const characterVariantForm = v.object({
  description: v.string(),
  defaultVariant: v.boolean(),
  age: v.number([v.integer()]),
  fromYear: v.number([v.integer()]),
  toYear: v.number([v.integer()]),
  previewFile: v.optional(v.instance(File)),
  previewImageUrl: v.optional(v.string()),
});

export type CharacterVariantFormObj = v.Input<typeof characterVariantForm>;
