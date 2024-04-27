import { SkillLanguageForm } from "@/components/skills/SkillLanguageForm";
import {
	Accordion,
	AccordionContent,
	AccordionItem,
	AccordionTrigger,
} from "@/components/ui/accordion";
import {
	Collapsible,
	CollapsibleContent,
	CollapsibleTrigger,
} from "@/components/ui/collapsible";
import { getSkillLanguages } from "@/lib/api/skills";
import { ChevronDown } from "lucide-react";
import { notFound } from "next/navigation";

export interface ISkillLanguageListProps {
	skillId: string;
}

export const SkillLanguageList = async (props: ISkillLanguageListProps) => {
	const { skillId } = props;

	try {
		const skillLanguages = await getSkillLanguages(skillId);

		<div>
			<h3>Languages:</h3>
			<div>
				<Collapsible className="group/collapsible mt-2">
					<CollapsibleTrigger>
						<div className="mb-3 flex flex-row gap-2 border p-2 rounded-md pr-4">
							<ChevronDown className="group-data-[state=open]/collapsible:rotate-180 " />{" "}
							Add Language
						</div>
					</CollapsibleTrigger>
					<CollapsibleContent>
						<SkillLanguageForm skillId={skillId} />
					</CollapsibleContent>
				</Collapsible>
				<Accordion type="multiple">
					{skillLanguages.map((l) => {
						return (
							<AccordionItem
								value={`locale-${l.locale}`}
								key={`locale-${l.locale}`}
								title={`Locale: ${l.locale}`}
							>
								<AccordionTrigger>
									<div>
										<p>
											{l.locale.toUpperCase()}: {l.name}
										</p>
										<p>Description: {l.description}</p>
									</div>
								</AccordionTrigger>
								<AccordionContent>
									<SkillLanguageForm skillId={skillId} language={l} />
								</AccordionContent>
							</AccordionItem>
						);
					})}
				</Accordion>
			</div>
		</div>;
	} catch (err) {
		console.error(err);
		return <div>Failed to load languages...</div>;
	}
};
