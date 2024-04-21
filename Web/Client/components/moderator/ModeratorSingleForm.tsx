"use client";
import { valibotResolver } from "@hookform/resolvers/valibot";
import axios from "axios";
import { useForm } from "react-hook-form";
import { toast } from "sonner";
import * as v from "valibot";
import type { IModerator } from "../../lib/models/moderator/ModeratorModel";
import { toastError } from "../errors/ErrorHandler";
import { Button } from "../ui/button";
import { Checkbox } from "../ui/checkbox";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "../ui/form";
import { Input } from "../ui/input";
import { type ModeratorFormData, moderatorFormSchema } from "./validate";

export interface IModeratorSingleFormProps {
  onSuccess: (moderators: IModerator[]) => void;
  close: () => void;
}

export const ModeratorSingleForm = (props: IModeratorSingleFormProps) => {
  const { close, onSuccess } = props;

  const form = useForm<ModeratorFormData>({
    resolver: valibotResolver(moderatorFormSchema),
    defaultValues: {
      permissions: {
        allPermissions: true,
        manageModerators: false,
        manageCharacterInfo: false,
        manageCharacterNotes: false,
        manageCharacters: false,
        manageSkillInfo: false,
        manageSkills: false,
        manageSkillTranslations: false,
      },
    },
  });

  const onSubmit = async (values: ModeratorFormData) => {
    try {
      const res = await axios.post("/api/moderator", values);
      if (res.status === 200) {
        toast("Successfully added a moderator");
        onSuccess(res.data as IModerator[]);
      }
    } catch (err) {
      toastError(err);
    }
  };

  return (
    <Form {...form}>
      <form onSubmit={form.handleSubmit(onSubmit)}>
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
          name="connectionSource"
          render={({ field }) => (
            <FormItem className="pr-2 pl-2">
              <FormLabel>Connection Source</FormLabel>
              <FormControl>
                <Input {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />

        <FormField
          control={form.control}
          name="connectionId"
          render={({ field }) => (
            <FormItem className="pr-2 pl-2">
              <FormLabel>Connection Id</FormLabel>
              <FormControl>
                <Input {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />

        <ul className="mt-4 flex flex-col gap-1 pr-2 pl-2">
          {Object.keys(form.getValues().permissions).map((key, i) => {
            return (
              <FormField
                key={`permission-${key}`}
                control={form.control}
                // biome-ignore lint/suspicious/noExplicitAny: <explanation>
                name={`permission.${key}` as any}
                render={({ field }) => (
                  <FormItem className="flex flex-row items-start space-x-3 space-y-0">
                    <FormControl>
                      <Checkbox
                        checked={field.value}
                        onCheckedChange={(checked) => {
                          field.onChange(checked);
                        }}
                      />
                    </FormControl>
                    <FormLabel className="font-normal text-sm">
                      {toSentence(key)}
                    </FormLabel>
                    <FormMessage />
                  </FormItem>
                )}
              />
            );
          })}
        </ul>

        <div className="mt-4 flex flex-row gap-2">
          <Button
            type="submit"
            disabled={
              !form.formState.dirtyFields || form.formState.isSubmitting
            }
          >
            Submit
          </Button>
          <Button
            type="reset"
            disabled={
              !form.formState.dirtyFields || form.formState.isSubmitting
            }
          >
            Reset
          </Button>
          <Button variant="secondary" onClick={close}>
            Cancel
          </Button>
        </div>
      </form>
    </Form>
  );
};

export const toSentence = (header: string) => {
  const newHeader: string[] = [];
  const chars = Array.from(header);
  for (const char of chars) {
    if (char.toUpperCase() && char !== char.toLowerCase()) {
      newHeader.push(" ");
      newHeader.push(char);
    } else {
      newHeader.push(char);
    }
    newHeader[0] = newHeader[0].toUpperCase();
  }
  const result = newHeader.join("");
  return result;
};
