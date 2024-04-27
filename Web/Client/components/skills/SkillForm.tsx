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
import { Progress } from "@/components/ui/progress";
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
import { actionCreateSkill, actionUpdateSkill } from "@/lib/api/actions/skills";
import { NEXT_PUBLIC_API_URL } from "@/lib/env";
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

	const form = useForm<SkillFormObj>({
		resolver: valibotResolver(skillForm),
		defaultValues: {
			name: skill?.name,
			description: skill?.description,
			category: skill?.category,
			summon: skill?.summon,
			ratingPhysical: skill?.ratingPhysical ?? 0,
			ratingMagical: skill?.ratingMagical ?? 0,
			costBuy: skill?.costBuy ?? 0,
			costUpgrade: skill?.costUpgrade ?? 0,
			costMaster: skill?.costMaster ?? 0,
			iconUrl: skill?.iconUrl ?? "",
			previewImageUrl: skill?.previewImageUrl ?? "",
		},
	});

	const cancelUploads = useRef(new AbortController());
	const [iconFileProgress, setIconFileProgress] =
		useState<AxiosProgressEvent | null>(null);
	const [previewFileProgress, setPreviewFileProgress] =
		useState<AxiosProgressEvent | null>(null);

	const onSubmit = async (values: SkillFormObj) => {
		const { iconFile, previewFile, ...newSkill } = values;

		const skillId = skill?.id ?? null;

		let newSkillId: string | null = null;
		try {
			if (skillId == null) {
				const createdSkill = await actionCreateSkill(newSkill as ISkill);
				toast("Skill Created");
				newSkillId = createdSkill.id.toString();
			} else {
				if (!_.isEqual(newSkill, skill)) {
					await actionUpdateSkill(skillId.toString(), newSkill as ISkill);
					toast("Skill Updated.");
				}
			}

			if (iconFile != null && (!isNull(skillId) || !isNull(newSkillId))) {
				const iconForm = new FormData();
				iconForm.append("iconFile", iconFile);

				try {
					await axios.postForm(
						`${NEXT_PUBLIC_API_URL}/api/skill/${
							newSkillId == null ? skillId : newSkillId
						}/images/icon`,
						iconForm,
						{
							onDownloadProgress: (prog) => {
								setIconFileProgress({ ...prog });
							},
							signal: cancelUploads.current.signal,
							withCredentials: true,
						},
					);

					setIconFileProgress(null);
					toast("Icon Uploaded.");
				} catch (err) {
					toastError(err);
				}
			}

			if (previewFile != null && (!isNull(skillId) || !isNull(newSkillId))) {
				const previewForm = new FormData();
				previewForm.append("previewFile", previewFile);
				try {
					await axios.postForm(
						`${NEXT_PUBLIC_API_URL}/api/skill/${
							newSkillId ?? skillId
						}/images/preview`,
						previewForm,
						{
							onDownloadProgress: (prog) => {
								setPreviewFileProgress({ ...prog });
							},
							signal: cancelUploads.current.signal,
							withCredentials: true,
						},
					);
					setPreviewFileProgress(null);

					toast("Preview Image Uploaded.");
				} catch (err) {
					toastError(err);
				}
			}

			if (newSkillId) {
				router.replace(`/dashboard/skills/${newSkillId}`);
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
										field.onChange(Number.parseInt(event));
									}}
									defaultValue={`${field.value}`}
								>
									<FormControl>
										<SelectTrigger>
											<SelectValue placeholder="Select a Category" />
										</SelectTrigger>
									</FormControl>
									<SelectContent>
										{skillCategoryList.map((l) => (
											<SelectItem value={`${l}`} key={`source-${l}`}>
												{SkillCategory[l]}
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
										field.onChange(Number.parseInt(event));
									}}
									defaultValue={`${field.value}`}
								>
									<FormControl>
										<SelectTrigger>
											<SelectValue placeholder="Select a Summon" />
										</SelectTrigger>
									</FormControl>
									<SelectContent>
										{summonList.map((l) => (
											<SelectItem value={`${l}`} key={`source-${l}`}>
												{SkillSummon[l]}
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
										<Input
											{...field}
											defaultValue={field.value.toString()}
											onChange={(event) => {
												field.onChange(
													Number.parseInt(event.currentTarget.value),
												);
											}}
											type="number"
											min={0}
											max={10}
											step={1}
										/>
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
										<Input
											{...field}
											defaultValue={field.value.toString()}
											onChange={(event) => {
												field.onChange(
													Number.parseInt(event.currentTarget.value),
												);
											}}
											type="number"
											min={0}
											max={10}
											step={1}
										/>
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
									<Input
										{...field}
										defaultValue={field.value.toString()}
										onChange={(event) => {
											field.onChange(
												Number.parseInt(event.currentTarget.value),
											);
										}}
										type="number"
									/>
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
									<Input
										{...field}
										defaultValue={field.value.toString()}
										onChange={(event) => {
											field.onChange(
												Number.parseInt(event.currentTarget.value),
											);
										}}
										type="number"
									/>
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
									<Input
										{...field}
										defaultValue={field.value.toString()}
										onChange={(event) => {
											field.onChange(
												Number.parseInt(event.currentTarget.value),
											);
										}}
										type="number"
									/>
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
					<Collapsible open={iconFileProgress != null}>
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
