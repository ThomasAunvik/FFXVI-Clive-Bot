import {
	type CharacterFormObj,
	characterForm,
} from "@/components/characters/validate";
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
import { UploadProgress } from "@/components/upload/UploadProgress";
import { NEXT_PUBLIC_API_URL } from "@/lib/env";
import type { ICharacter } from "@/lib/models/characters/CharacterModel";
import { valibotResolver } from "@hookform/resolvers/valibot";
import axios, { type AxiosProgressEvent } from "axios";
import _, { isNull } from "lodash";
import { LoaderCircle } from "lucide-react";
import { useRouter } from "next/navigation";
import { useRef, useState } from "react";
import { useForm } from "react-hook-form";
import { toast } from "sonner";

export interface ICharacterFormProps {
	characterId?: string;
	character?: ICharacter;
}

export const CharacterForm = (props: ICharacterFormProps) => {
	const { character, characterId } = props;

	const cancelUploads = useRef(new AbortController());
	const [previewFileProgress, setPreviewFileProgress] =
		useState<AxiosProgressEvent | null>(null);

	const form = useForm<CharacterFormObj>({
		resolver: valibotResolver(characterForm),
		defaultValues: {
			name: character?.name ?? "",
		},
	});

	const onSubmit = async (values: CharacterFormObj) => {
		const { previewFile, ...newCharacter } = values;

		const router = useRouter();

		if (!characterId) {
			const res = await axios.post(
				`${NEXT_PUBLIC_API_URL}/api/character/`,
				newCharacter,
			);

			const newInitialCharacter = res.data as ICharacter;
			form.reset(newInitialCharacter);

			toast("Sucessfully created character");
		} else {
			const res = await axios.put(
				`${NEXT_PUBLIC_API_URL}/api/character/${characterId}`,
				newCharacter,
			);

			const newInitialCharacter = res.data as ICharacter;
			form.reset(newInitialCharacter);

			toast("Sucessfully updated character");
		}

		if (previewFile != null && !isNull(characterId)) {
			const previewForm = new FormData();
			previewForm.append("previewFile", previewFile);
			await axios.postForm(
				`${NEXT_PUBLIC_API_URL}/api/character/${characterId}/images/preview`,
				previewForm,
				{
					onDownloadProgress: (prog) => {
						setPreviewFileProgress({ ...prog });
					},
					signal: cancelUploads.current.signal,
				},
			);

			toast("Sucessfully uploaded Preview Image");
		}

		if (
			(isNull(character) || character === undefined) &&
			!isNull(characterId)
		) {
			router.replace(`/dashboard/characters/${characterId}`);
		}

		return;
	};

	return (
		<div>
			<h1>{character?.name ?? "New Character"}</h1>
			<Form {...form}>
				<form
					onSubmit={form.handleSubmit(onSubmit)}
					className="grap-6 flex flex-col"
				>
					<div>
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
					</div>
					<div className="flex flex-col gap-2">
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
								variant="secondary"
								onClick={() => {
									cancelUploads.current.abort();
								}}
							>
								Cancel Upload
							</Button>
						) : null}

						{!form.formState.isSubmitting ? null : (
							<div>
								<LoaderCircle className="animate-spin" />
								<span className="sr-only">Loading...</span>
							</div>
						)}
					</div>
				</form>
			</Form>
		</div>
	);
};
