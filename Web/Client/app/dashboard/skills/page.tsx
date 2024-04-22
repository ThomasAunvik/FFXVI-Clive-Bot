import { SkillGeneralList } from "@/components/skills/SkillGeneralList";
import { SkillSummonList } from "@/components/skills/SkillSummonList";
import { Button } from "@/components/ui/button";
import { getSkills } from "@/lib/api/skills";
import { PlusIcon } from "lucide-react";
import Link from "next/link";

const SkillPage = async () => {
  const skills = await getSkills();

  return (
    <div className="flex flex-col gap-4">
      <Link href={"/dashboard/skills/new"}>
        <Button className="mb-3">
          <PlusIcon />
        </Button>
      </Link>
      <div className="flex flex-row gap-8">
        <div className="max-w-sm flex-1">
          <SkillSummonList skills={skills} />
        </div>
        <div className="max-w-sm flex-1">
          <SkillGeneralList skills={skills} />
        </div>
      </div>
    </div>
  );
};

export default SkillPage;
