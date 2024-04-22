"use client";
import { toastError } from "@/components/errors/ErrorHandler";
import {
  type ModeratorFormData,
  moderatorFormSchema,
} from "@/components/moderator/validate";
import { Button } from "@/components/ui/button";
import { Checkbox } from "@/components/ui/checkbox";
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
import type { IModerator } from "@/lib/models/moderator/ModeratorModel";
import { valibotResolver } from "@hookform/resolvers/valibot";
import axios from "axios";
import { useForm } from "react-hook-form";
import { toast } from "sonner";

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
      <form
        onSubmit={form.handleSubmit(onSubmit)}
        className="flex flex-col gap-2"
      >
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
              <Select onValueChange={field.onChange} defaultValue={field.value}>
                <FormControl>
                  <SelectTrigger>
                    <SelectValue placeholder="Select a Source" />
                  </SelectTrigger>
                </FormControl>
                <SelectContent>
                  {["Discord"].map((l) => (
                    <SelectItem value={l} key={`source-${l}`}>
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

        <ul className="mt-2 flex flex-col gap-1 pr-2 pl-2">
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

        <div className="mt-2 flex flex-row gap-2">
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
