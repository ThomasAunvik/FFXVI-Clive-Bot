import { SkillForm } from "@/components/skills/SkillForm";
import { SkillLanguageList } from "@/components/skills/SkillLanguagesList";
import { Button } from "@/components/ui/button";
import { getSkill } from "@/lib/api/skills";
import { LoaderCircle } from "lucide-react";
import Link from "next/link";
import { notFound } from "next/navigation";
import { Suspense } from "react";

interface SkillPageProps {
	params: { skillId: string };
}

const SkillPage = async (props: SkillPageProps) => {
	const { params } = props;

	return (
		<div className="flex flex-col gap-4">
			<Link href={"/dashboard/skills"}>
				<Button>Return to Skills</Button>
			</Link>
			<div className="flex flex-row gap-4">
				<div className="max-w-md flex-1">
					<Suspense fallback={<LoaderCircle className="animate-spin" />}>
						<SkillFormServer params={params} />
					</Suspense>
				</div>
				<div className="max-w-md flex-1">
					<Suspense fallback={<LoaderCircle className="animate-spin" />}>
						<SkillLanguageList skillId={params.skillId.toString()} />
					</Suspense>
				</div>
			</div>
		</div>
	);
};

const SkillFormServer = async (props: SkillPageProps) => {
	try {
		const skill = await getSkill(props.params.skillId);

		return <SkillForm skill={skill} />;
	} catch (err) {
		return notFound();
	}
};

export default SkillPage;
