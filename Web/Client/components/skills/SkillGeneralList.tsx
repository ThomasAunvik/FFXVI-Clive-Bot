import { replaceCDN } from "@/components/constants";
import {
  Accordion,
  AccordionContent,
  AccordionItem,
  AccordionTrigger,
} from "@/components/ui/accordion";
import { Skeleton } from "@/components/ui/skeleton";
import { type ISkill, SkillSummon } from "@/lib/models/skill/SkillModel";
import { LoaderCircle } from "lucide-react";
import Image from "next/image";
import Link from "next/link";

export interface SkillGeneralListProps {
  skills: ISkill[];
}

export const SkillGeneralList = (props: SkillGeneralListProps) => {
  const { skills } = props;

  return (
    <div>
      <Accordion type="single" collapsible>
        <AccordionItem value="otherskills">
          <AccordionTrigger>General Skills</AccordionTrigger>
          <AccordionContent>
            {!skills ? (
              <div>
                <LoaderCircle className="animate-spin" />
                <span className="sr-only">Loading...</span>
              </div>
            ) : (
              <ul className="flex flex-col gap-4">
                {skills
                  .filter((s) => s.summon === SkillSummon.None)
                  .map((s) => {
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
                          <span className="mt-2 ml-4 text-base">{s.name}</span>
                        </Link>
                      </li>
                    );
                  })}
              </ul>
            )}
          </AccordionContent>
        </AccordionItem>
      </Accordion>
    </div>
  );
};
