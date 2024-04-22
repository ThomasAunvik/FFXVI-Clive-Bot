"use client";
import { toastError } from "@/components/errors/ErrorHandler";
import { type SkillFormObj, skillForm } from "@/components/skills/validate";
import { Button } from "@/components/ui/button";
import { Collapsible, CollapsibleContent } from "@/components/ui/collapsible";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { Slider } from "@/components/ui/slider";
import { Textarea } from "@/components/ui/textarea";
import { UploadProgress } from "@/components/upload/UploadProgress";
import {
  type ISkill,
  SkillCategory,
  SkillSummon,
  skillCategoryList,
  summonList,
} from "@/lib/models/skill/SkillModel";
import { valibotResolver } from "@hookform/resolvers/valibot";
import axios, { type AxiosProgressEvent } from "axios";
import _, { isNull } from "lodash";
import { LoaderCircle } from "lucide-react";
import { useRouter } from "next/navigation";
import { useRef, useState } from "react";
import { useForm } from "react-hook-form";
import { toast } from "sonner";

export interface ISkillFormProps {
  skill?: ISkill;
}

export const SkillForm = (props: ISkillFormProps) => {
  const { skill } = props;

  const router = useRouter();

  const [initialSkill, setInitialSkill] = useState<ISkill>(
    skill ?? {
      id: 0,
      name: "",
      description: "",
      localized: [],
      category: SkillCategory.None,
      summon: SkillSummon.Ifrit,
      ratingPhysical: 0,
      ratingMagical: 0,
      costBuy: 0,
      costUpgrade: 0,
      costMaster: 0,
    },
  );

  const form = useForm<SkillFormObj>({
    resolver: valibotResolver(skillForm),
    defaultValues: {
      name: initialSkill?.name,
      description: initialSkill?.description,
      category: initialSkill?.category,
      summon: initialSkill?.summon,
      ratingPhysical: initialSkill?.ratingPhysical,
      ratingMagical: initialSkill?.ratingMagical,
      costBuy: initialSkill?.costBuy,
      costUpgrade: initialSkill?.costUpgrade,
      costMaster: initialSkill?.costMaster,
      iconUrl: initialSkill?.iconUrl,
      previewImageUrl: initialSkill.previewImageUrl,
    },
  });

  const cancelUploads = useRef(new AbortController());
  const [iconFileProgress, setIconFileProgress] =
    useState<AxiosProgressEvent | null>(null);
  const [previewFileProgress, setPreviewFileProgress] =
    useState<AxiosProgressEvent | null>(null);

  const onSubmit = async (values: SkillFormObj) => {
    const { iconFile, previewFile, ...newSkill } = values;

    let skillId = skill?.id ?? null;
    try {
      if (skill == null) {
        const res = await axios.post("/api/skill/", newSkill);

        const newInitialSkill = res.data as ISkill;
        skillId = newInitialSkill.id;
        setInitialSkill(newInitialSkill);
        toast("Skill Created");
      } else {
        if (!_.isEqual(newSkill, initialSkill)) {
          const res = await axios.put(`/api/skill/${skill.id}`, newSkill);

          const newInitialSkill = res.data as ISkill;
          setInitialSkill(newInitialSkill);
          toast("Skill Updated.");
        }
      }

      if (iconFile != null && !isNull(skillId)) {
        const iconForm = new FormData();
        iconForm.append("iconFile", iconFile);

        try {
          await axios.postForm(`/api/skill/${skillId}/images/icon`, iconForm, {
            onDownloadProgress: (prog) => {
              setIconFileProgress({ ...prog });
            },
            signal: cancelUploads.current.signal,
          });
          toast("Icon Uploaded.");
        } catch (err) {
          toastError(err);
        }
      }

      if (previewFile != null && !isNull(skillId)) {
        const previewForm = new FormData();
        previewForm.append("previewFile", previewFile);
        try {
          await axios.postForm(
            `/api/skill/${skillId}/images/preview`,
            previewForm,
            {
              onDownloadProgress: (prog) => {
                setPreviewFileProgress({ ...prog });
              },
              signal: cancelUploads.current.signal,
            },
          );

          toast("Preview Image Uploaded.");
        } catch (err) {
          toastError(err);
        }
      }

      if ((isNull(skill) || skill === undefined) && !isNull(skillId)) {
        router.replace(`/dashboard/skills/${skillId}`);
      }
    } catch (err) {
      toastError(err);
    }

    return;
  };

  return (
    <Form {...form}>
      <form
        onSubmit={form.handleSubmit(onSubmit)}
        className="flex flex-col gap-6"
      >
        <div className="flex flex-col gap-2">
          <FormField
            control={form.control}
            name="name"
            render={({ field }) => (
              <FormItem className="pr-2 pl-2">
                <FormLabel>Name</FormLabel>
                <FormControl>
                  <Input {...field} />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />

          <FormField
            control={form.control}
            name="description"
            render={({ field }) => (
              <FormItem className="pr-2 pl-2">
                <FormLabel>Description</FormLabel>
                <FormControl>
                  <Textarea {...field} />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />

          <FormField
            control={form.control}
            name="category"
            render={({ field }) => (
              <FormItem className="pr-2 pl-2">
                <FormLabel>Category</FormLabel>
                <Select
                  onValueChange={(event) => {
                    field.onChange(Number.parseInt(event) as SkillCategory);
                  }}
                  defaultValue={field.value.toString()}
                >
                  <FormControl>
                    <SelectTrigger>
                      <SelectValue placeholder="Select a Category" />
                    </SelectTrigger>
                  </FormControl>
                  <SelectContent>
                    {skillCategoryList.map((l) => (
                      <SelectItem
                        value={SkillCategory[l].toString()}
                        key={`source-${l}`}
                      >
                        {l}
                      </SelectItem>
                    ))}
                  </SelectContent>
                </Select>
                <FormMessage />
              </FormItem>
            )}
          />

          <FormField
            control={form.control}
            name="summon"
            render={({ field }) => (
              <FormItem className="pr-2 pl-2">
                <FormLabel>Summon</FormLabel>
                <Select
                  onValueChange={(event) => {
                    field.onChange(Number.parseInt(event) as SkillSummon);
                  }}
                  defaultValue={field.value.toString()}
                >
                  <FormControl>
                    <SelectTrigger>
                      <SelectValue placeholder="Select a Summon" />
                    </SelectTrigger>
                  </FormControl>
                  <SelectContent>
                    {summonList.map((l) => (
                      <SelectItem
                        value={SkillSummon[l].toString()}
                        key={`source-${l}`}
                      >
                        {l}
                      </SelectItem>
                    ))}
                  </SelectContent>
                </Select>
                <FormMessage />
              </FormItem>
            )}
          />
        </div>

        <div className="flex flex-col gap-2">
          <FormField
            control={form.control}
            name="ratingPhysical"
            render={({ field }) => (
              <FormItem className="pr-2 pl-2">
                <FormLabel>Damage Rating</FormLabel>
                <div className="flex flex-row gap-4">
                  <FormControl>
                    <Input {...field} type="number" min={0} max={10} step={1} />
                  </FormControl>
                  <FormControl>
                    <Slider
                      defaultValue={[field.value]}
                      onValueChange={(e) => {
                        field.onChange(e[0]);
                      }}
                      onBlur={field.onBlur}
                      min={0}
                      max={10}
                      step={1}
                    />
                  </FormControl>
                </div>
                <FormMessage />
              </FormItem>
            )}
          />
          <FormField
            control={form.control}
            name="ratingMagical"
            render={({ field }) => (
              <FormItem className="pr-2 pl-2">
                <FormLabel>Stagger Rating</FormLabel>
                <div className="flex flex-row gap-4">
                  <FormControl>
                    <Input {...field} type="number" min={0} max={10} step={1} />
                  </FormControl>
                  <FormControl>
                    <Slider
                      defaultValue={[field.value]}
                      onValueChange={(e) => {
                        field.onChange(e[0]);
                      }}
                      onBlur={field.onBlur}
                      min={0}
                      max={10}
                      step={1}
                    />
                  </FormControl>
                </div>
                <FormMessage />
              </FormItem>
            )}
          />
        </div>

        <div className="flex flex-col gap-2">
          <FormField
            control={form.control}
            name="costBuy"
            render={({ field }) => (
              <FormItem className="pr-2 pl-2">
                <FormLabel>Cost to Buy</FormLabel>
                <FormControl>
                  <Input {...field} type="number" />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />

          <FormField
            control={form.control}
            name="costUpgrade"
            render={({ field }) => (
              <FormItem className="pr-2 pl-2">
                <FormLabel>Cost to Upgrade</FormLabel>
                <FormControl>
                  <Input {...field} type="number" />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />

          <FormField
            control={form.control}
            name="costMaster"
            render={({ field }) => (
              <FormItem className="pr-2 pl-2">
                <FormLabel>Cost to Master</FormLabel>
                <FormControl>
                  <Input {...field} type="number" />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />
        </div>

        <div className="flex flex-col gap-2">
          <FormField
            control={form.control}
            name="iconFile"
            render={({ field }) => (
              <FormItem className="pr-2 pl-2">
                <FormLabel>Icon</FormLabel>
                <FormControl>
                  <Input
                    type="file"
                    accept="image/*"
                    onChange={(event) => {
                      const files = event.currentTarget.files;
                      if (!files || files.length <= 0) return;

                      field.onChange(files[0]);
                    }}
                    onBlur={field.onBlur}
                  />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />
          <Collapsible open={previewFileProgress != null}>
            <CollapsibleContent>
              <UploadProgress progress={iconFileProgress} />
            </CollapsibleContent>
          </Collapsible>

          <FormField
            control={form.control}
            name="iconUrl"
            render={({ field }) => (
              <FormItem className="pr-2 pl-2">
                <FormLabel>Icon Url</FormLabel>
                <FormControl>
                  <Input {...field} />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />

          <FormField
            control={form.control}
            name="previewFile"
            render={({ field }) => (
              <FormItem className="pr-2 pl-2">
                <FormLabel>Preview Image</FormLabel>
                <FormControl>
                  <Input
                    type="file"
                    accept="image/*"
                    onChange={(event) => {
                      const files = event.currentTarget.files;
                      if (!files || files.length <= 0) return;

                      field.onChange(files[0]);
                    }}
                    onBlur={field.onBlur}
                  />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />

          <Collapsible open={previewFileProgress != null}>
            <CollapsibleContent>
              <UploadProgress progress={previewFileProgress} />
            </CollapsibleContent>
          </Collapsible>
          <FormField
            control={form.control}
            name="previewImageUrl"
            render={({ field }) => (
              <FormItem className="pr-2 pl-2">
                <FormLabel>Preview Image Url</FormLabel>
                <FormControl>
                  <Input {...field} />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />
        </div>
        <div className="flex flex-row gap-4">
          <Button
            type="submit"
            disabled={!form.formState.isDirty || form.formState.isSubmitting}
          >
            Submit
          </Button>

          {(form.getValues().iconFile != null ||
            form.getValues().previewFile != null) &&
          form.formState.isSubmitting ? (
            <Button
              type="button"
              onClick={() => {
                cancelUploads.current.abort();
              }}
            >
              Cancel Upload
            </Button>
          ) : null}

          {!form.formState.isSubmitting ? null : (
            <Button type="button" disabled>
              <LoaderCircle className="animate-spin" />
              <span className="sr-only">Loading...</span>
            </Button>
          )}
        </div>
      </form>
    </Form>
  );
};
