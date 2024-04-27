import { replaceCDN } from "@/components/constants";
import {
	Accordion,
	AccordionContent,
	AccordionItem,
	AccordionTrigger,
} from "@/components/ui/accordion";
import { Label } from "@/components/ui/label";
import { Skeleton } from "@/components/ui/skeleton";
import {
	type ISkill,
	SkillSummon,
	summonList,
} from "@/lib/models/skill/SkillModel";
import { LoaderCircle } from "lucide-react";
import Image from "next/image";
import Link from "next/link";

export interface SkillSummonListProps {
	skills: ISkill[];
}

export const SkillSummonList = (props: SkillSummonListProps) => {
	const { skills } = props;

	return (
		<div>
			<Accordion type="multiple">
				{summonList
					.filter((s) => s !== 0)
					.map((s, i) => {
						const summonSkills = skills.filter((sk) => sk.summon === s);

						return (
							<AccordionItem
								value={i.toString()}
								key={`summon-${s.toString()}`}
							>
								<AccordionTrigger>{SkillSummon[s]}</AccordionTrigger>
								<AccordionContent>
									{skills === undefined ? (
										<div>
											<LoaderCircle className="animate-spin" />
											<span className="sr-only">Loading...</span>
										</div>
									) : (
										<ul className="flex flex-col gap-4">
											{summonSkills.length <= 0 ? (
												<Label>There are no skills</Label>
											) : null}
											{summonSkills.map((s) => {
												return (
													<li key={`skill-${s.id}`}>
														<Link
															href={`/dashboard/skills/${s.id}`}
															className="flex flex-row"
														>
															{s.iconUrl ? (
																<Image
																	alt={`Skill ${s.name} Image`}
																	src={replaceCDN(s.iconUrl)}
																	width={40}
																	height={40}
																/>
															) : (
																<Skeleton className="h-10 w-10" />
															)}
															<span className="mt-2 ml-4 text-base">
																{s.name}
															</span>
														</Link>
													</li>
												);
											})}
										</ul>
									)}
								</AccordionContent>
							</AccordionItem>
						);
					})}
			</Accordion>
		</div>
	);
};
