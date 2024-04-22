"use client";
import { toastError } from "@/components/errors/ErrorHandler";
import {
	type SkillLanguageFormObj,
	skillLanguageForm,
} from "@/components/skills/validate";
import { Button } from "@/components/ui/button";
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
import { NEXT_PUBLIC_API_URL } from "@/lib/env";
import type { ISkillLanguage } from "@/lib/models/skill/SkillLanguageModel";
import { valibotResolver } from "@hookform/resolvers/valibot";
import axios from "axios";
import { isUndefined } from "lodash";
import { LoaderCircle, SaveIcon, TrashIcon } from "lucide-react";
import { useState } from "react";
import { useForm } from "react-hook-form";

export interface ISkillLanguageFormProps {
	skillId: string;
	language?: ISkillLanguage;
	onDelete?: () => void;
	onUpdate: (languages: ISkillLanguage[]) => void;
}

export const SkillLanguageForm = (props: ISkillLanguageFormProps) => {
	const { skillId, language, onDelete, onUpdate } = props;

	const [initialValues, setInitialValues] = useState<ISkillLanguage>(
		language ??
			({
				id: 0,
				locale: "",
				name: "New Skill Name",
				description: "New Skill Description",
			} as ISkillLanguage),
	);

	const form = useForm<SkillLanguageFormObj>({
		resolver: valibotResolver(skillLanguageForm),
		defaultValues: {
			name: initialValues.name,
			locale: initialValues.locale,
			description: initialValues.description,
		},
	});

	const onSubmit = async (values: SkillLanguageFormObj) => {
		try {
			const res = await axios.post(
				`${NEXT_PUBLIC_API_URL}/api/skill/${skillId}/languages/${values.locale}`,
				values,
			);
			if (res.status === 200) {
				const data = res.data as ISkillLanguage[];
				onUpdate(data);
				if (isUndefined(language)) {
					const newLang = {
						id: 0,
						locale: "",
						name: "New Skill Name",
						description: "New Skill Description",
					} as ISkillLanguage;
					setInitialValues(newLang);

					form.reset(newLang);
				}
			}
		} catch (err) {
			toastError(err);
		}
	};

	return (
		<Form {...form}>
			<form onSubmit={form.handleSubmit(onSubmit)}>
				<div className="mb-3 flex flex-row gap-2">
					{onDelete == null ? null : (
						<div>
							<Button type="button" onClick={onDelete}>
								<TrashIcon />
							</Button>
						</div>
					)}

					<div style={{ flex: 0 }}>
						<Button
							type="submit"
							disabled={!form.formState.isDirty || form.formState.isSubmitting}
						>
							{form.formState.isSubmitting ? (
								<div>
									<LoaderCircle className="animate-spin" />
									<span className="sr-only">Loading...</span>
								</div>
							) : (
								<SaveIcon />
							)}
						</Button>
					</div>
				</div>
				<FormField
					control={form.control}
					name="locale"
					render={({ field }) => (
						<FormItem className="pr-2 pl-2">
							<FormLabel>Locale</FormLabel>
							<Select onValueChange={field.onChange} defaultValue={field.value}>
								<FormControl>
									<SelectTrigger>
										<SelectValue placeholder="Select a Language" />
									</SelectTrigger>
								</FormControl>
								<SelectContent>
									{["en", "no", "jp", "de", "fr"].map((l) => (
										<SelectItem value={l} key={`locale-${l}`}>
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
					name="name"
					render={({ field }) => (
						<FormItem className="pr-2 pl-2">
							<FormLabel>Skill Name</FormLabel>
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
							<FormLabel>Skill Description</FormLabel>
							<FormControl>
								<Input {...field} />
							</FormControl>
							<FormMessage />
						</FormItem>
					)}
				/>
			</form>
		</Form>
	);
};
