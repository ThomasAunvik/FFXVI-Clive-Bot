import { SkillOtherList } from "@/components/skills/SkillOtherList";
import { SkillSummonList } from "@/components/skills/SkillSummonList";
import { Button } from "@/components/ui/button";
import { PlusIcon } from "lucide-react";
import Link from "next/link";

const SkillPage = () => {
  return (
    <div className="flex flex-col gap-4">
      <Link href={"/dashboard/skills/new"}>
        <Button className="mb-3">
          <PlusIcon />
        </Button>
      </Link>
      <div className="flex flex-row gap-8">
        <div className="max-w-sm flex-1">
          <SkillSummonList />
        </div>
        <div className="max-w-sm flex-1">
          <SkillOtherList />
        </div>
      </div>
    </div>
  );
};

export default SkillPage;
