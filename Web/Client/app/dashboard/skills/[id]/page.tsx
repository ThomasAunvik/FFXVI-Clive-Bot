"use client";
import { toastError } from "@/components/errors/ErrorHandler";
import useIsMounted from "@/components/misc/useIsMounted";
import { SkillForm } from "@/components/skills/SkillForm";
import { SkillLanguageList } from "@/components/skills/SkillLanguagesList";
import { Button } from "@/components/ui/button";
import type { ISkill } from "@/lib/models/skill/SkillModel";
import axios from "axios";
import { LoaderCircle } from "lucide-react";
import Link from "next/link";
import { useCallback, useEffect, useState } from "react";

interface SkillPageProps {
  params: { id: string };
}

const SkillPage = (props: SkillPageProps) => {
  const { params } = props;

  const [skill, setSkill] = useState<ISkill | null>(null);

  const mounted = useIsMounted();

  const fetchSkill = useCallback(async (skillId: string) => {
    try {
      const res = await axios.get(`/api/skill/${skillId}`);
      if (res.status !== 200) return;

      setSkill(res.data as ISkill);
    } catch (err) {
      toastError(err);
    }
  }, []);

  useEffect(() => {
    if (!params.id) return;
    if (!mounted) return;

    fetchSkill(params.id.toString());
  }, [fetchSkill, mounted, params.id]);

  return (
    <div className="flex flex-col gap-4">
      <Link href={"/dashboard/skills"}>
        <Button>Return to Skills</Button>
      </Link>
      <div className="flex flex-row gap-4">
        <div className="max-w-md flex-1">
          {skill == null ? (
            <div>
              <LoaderCircle className="animate-spin" />
              <span className="sr-only">Loading...</span>
            </div>
          ) : (
            <SkillForm skill={skill} />
          )}
        </div>
        <div className="max-w-md flex-1">
          {skill?.id == null ? (
            <div>
              <LoaderCircle className="animate-spin" />
              <span className="sr-only">Loading...</span>
            </div>
          ) : (
            <SkillLanguageList skillId={skill.id.toString()} />
          )}
        </div>
      </div>
    </div>
  );
};

export default SkillPage;
