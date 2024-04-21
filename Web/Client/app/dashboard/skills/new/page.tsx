import { SkillForm } from "@/components/skills/SkillForm";
import { Button } from "@/components/ui/button";
import Link from "next/link";

const SkillPage = () => {
  return (
    <div className="flex flex-col gap-4">
      <Link href={"/dashboard/skills"}>
        <Button>Return to Skills</Button>
      </Link>
      <div className="max-w-md flex-1">
        <SkillForm />
      </div>
    </div>
  );
};

export default SkillPage;
