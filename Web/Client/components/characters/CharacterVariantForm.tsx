import {
	type CharacterVariantFormObj,
	characterVariantForm,
} from "@/components/characters/validate";
import { toastError } from "@/components/errors/ErrorHandler";
import { Button } from "@/components/ui/button";
import { Checkbox } from "@/components/ui/checkbox";
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
import { Textarea } from "@/components/ui/textarea";
import { UploadProgress } from "@/components/upload/UploadProgress";
import { NEXT_PUBLIC_API_URL } from "@/lib/env";
import type { ICharacter } from "@/lib/models/characters/CharacterModel";
import type { ICharacterVariant } from "@/lib/models/characters/CharacterVariant";
import { valibotResolver } from "@hookform/resolvers/valibot";
import axios, { type AxiosProgressEvent } from "axios";
import _, { isNull } from "lodash";
import { LoaderCircle } from "lucide-react";
import { useRef, useState } from "react";
import { useForm } from "react-hook-form";

interface CharacterVariantFormProps {
	character: ICharacter;
	variant?: ICharacterVariant;
	onUpdated: (variant: ICharacterVariant) => void;
	onCancel?: () => void;
}

export const CharacterVariantForm = (props: CharacterVariantFormProps) => {
	const { variant, character, onUpdated } = props;

	const cancelUploads = useRef(new AbortController());
	const [previewFileProgress, setPreviewFileProgress] =
		useState<AxiosProgressEvent | null>(null);

	const form = useForm<CharacterVariantFormObj>({
		resolver: valibotResolver(characterVariantForm),
		defaultValues: {
			age: variant?.age,
			defaultVariant: variant?.defaultVariant,
			description: variant?.description,
			fromYear: variant?.fromYear,
			toYear: variant?.toYear,
			previewImageUrl: variant?.previewImageUrl,
		},
	});

	const onSubmit = async (values: CharacterVariantFormObj) => {
		const { previewFile, ...newVariant } = values;

		let variantId = variant?.id ?? null;
		try {
			if (variant == null) {
				const res = await axios.post(
					`${NEXT_PUBLIC_API_URL}/api/character/${character.id}/variant`,
					newVariant,
				);

				const newVariantData = res.data as ICharacterVariant;
				variantId = newVariantData.id;
				onUpdated(newVariantData);
			} else {
				if (!_.isEqual(newVariant, variant)) {
					const res = await axios.put(
						`${NEXT_PUBLIC_API_URL}/api/character/${character.id}/variant/${variant.id}`,
						newVariant,
					);

					const newVariantData = res.data as ICharacterVariant;
					onUpdated(newVariantData);
				}
			}

			if (previewFile != null && !isNull(variantId)) {
				const previewForm = new FormData();
				previewForm.append("previewFile", previewFile);
				const res = await axios.postForm(
					`${NEXT_PUBLIC_API_URL}/api/character/${character.id}/variant/${variantId}/images/preview`,
					previewForm,
					{
						onDownloadProgress: (prog) => {
							setPreviewFileProgress({ ...prog });
						},
						signal: cancelUploads.current.signal,
					},
				);
			}
		} catch (err) {
			toastError(err);
		}
	};

	return (
		<Form {...form}>
			<form onSubmit={form.handleSubmit(onSubmit)}>
				<div>
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
						name="defaultVariant"
						render={({ field }) => (
							<FormItem className="flex flex-row items-start space-x-3 space-y-0">
								<FormControl>
									<Checkbox
										checked={field.value}
										onCheckedChange={field.onChange}
									/>
								</FormControl>
								<FormLabel className="font-normal text-sm">
									Default Variant
								</FormLabel>
								<FormMessage />
							</FormItem>
						)}
					/>

					<FormField
						control={form.control}
						name="age"
						render={({ field }) => (
							<FormItem className="pr-2 pl-2">
								<FormLabel>Age</FormLabel>
								<FormControl>
									<Input {...field} type="number" />
								</FormControl>
								<FormMessage />
							</FormItem>
						)}
					/>
				</div>
				<div>
					<FormField
						control={form.control}
						name="fromYear"
						render={({ field }) => (
							<FormItem className="pr-2 pl-2">
								<FormLabel>From Year</FormLabel>
								<FormControl>
									<Input {...field} type="number" />
								</FormControl>
								<FormMessage />
							</FormItem>
						)}
					/>
					<FormField
						control={form.control}
						name="toYear"
						render={({ field }) => (
							<FormItem className="pr-2 pl-2">
								<FormLabel>To Year</FormLabel>
								<FormControl>
									<Input {...field} type="number" />
								</FormControl>
								<FormMessage />
							</FormItem>
						)}
					/>
				</div>
				<div>
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

					{form.getValues().previewFile != null &&
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
