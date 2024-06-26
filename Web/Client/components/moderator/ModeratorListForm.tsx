"use client";
import { toastError } from "@/components/errors/ErrorHandler";
import { toSentence } from "@/components/moderator/ModeratorSingleForm";
import {
	type ModeratorFormData,
	moderatorFormSchema,
} from "@/components/moderator/validate";
import {
	Accordion,
	AccordionContent,
	AccordionItem,
} from "@/components/ui/accordion";
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
import {
	actionDeleteModerator,
	actionEditModerator,
} from "@/lib/api/actions/moderators";
import { NEXT_PUBLIC_API_URL } from "@/lib/env";
import type { IModerator } from "@/lib/models/moderator/ModeratorModel";
import { valibotResolver } from "@hookform/resolvers/valibot";
import axios from "axios";
import { LoaderCircle, PencilIcon, SaveIcon, TrashIcon } from "lucide-react";
import { useState } from "react";
import { useForm } from "react-hook-form";
import { toast } from "sonner";

export interface IModeratorListFormProps {
	moderator: IModerator;
	onUpdate: (moderators: IModerator[]) => void;
}

export const ModeratorListForm = (props: IModeratorListFormProps) => {
	const { moderator, onUpdate } = props;

	const [collapse, setCollapse] = useState(true);

	const form = useForm<ModeratorFormData>({
		resolver: valibotResolver(moderatorFormSchema),
		defaultValues: {
			name: moderator.name,
			connectionId: moderator.connectionId,
			connectionSource: moderator.connectionSource,
			permissions: moderator.permissions,
		},
	});

	const onSubmit = async (values: ModeratorFormData) => {
		try {
			const res = await actionEditModerator(
				moderator.id.toString(),
				values as IModerator,
			);

			toast("Successfully added a moderator");
			onUpdate(res);
		} catch (err) {
			toastError(err);
		}
	};

	const onDelete = async () => {
		try {
			const res = await actionDeleteModerator(moderator.id.toString());

			toast("Successfully added a moderator");
			onUpdate(res);
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
				<div className="flex flex-row justify-end gap-2">
					<Button type="button" onClick={onDelete}>
						<TrashIcon />
					</Button>
					<Button
						variant="secondary"
						type="button"
						onClick={() => setCollapse(!collapse)}
					>
						<PencilIcon />
					</Button>
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

				<Accordion type="single" value={collapse ? "0" : "namefields"}>
					<AccordionItem value="namefields" className="border-none">
						<AccordionContent>
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
										<Select
											onValueChange={field.onChange}
											defaultValue={field.value}
										>
											<FormControl>
												<SelectTrigger>
													<SelectValue placeholder="Select a Language" />
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
						</AccordionContent>
					</AccordionItem>
				</Accordion>

				<ul className="mt-2 flex flex-col gap-1 pr-2 pl-2">
					{Object.keys(form.getValues().permissions).map((key, i) => {
						if (key === "id" || key === "moderator") return null;

						return (
							<FormField
								key={`permission-${key}`}
								control={form.control}
								// biome-ignore lint/suspicious/noExplicitAny: <explanation>
								{...form.register(`permissions.${key}` as any)}
								render={({ field }) => (
									<FormItem className="flex flex-row items-start space-x-3 space-y-0">
										<FormControl>
											<Checkbox
												checked={field.value}
												onCheckedChange={field.onChange}
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
			</form>
		</Form>
	);
};
