"use client";
import axios from "axios";
import { LoaderCircle } from "lucide-react";
import Image from "next/image";
import Link from "next/link";
import { useEffect, useState } from "react";
import { type ISkill, SkillSummon } from "../../lib/models/skill/SkillModel";
import { replaceCDN } from "../constants";
import { toastError } from "../errors/ErrorHandler";
import {
  Accordion,
  AccordionContent,
  AccordionItem,
  AccordionTrigger,
} from "../ui/accordion";
import { Skeleton } from "../ui/skeleton";

export const SkillOtherList = () => {
  const [skills, setSkills] = useState<ISkill[] | null>(null);

  const fetchOtherSkills = async () => {
    if (skills) return;

    try {
      const params = new URLSearchParams({
        summon: SkillSummon[SkillSummon.None].toString(),
      });

      const res = await axios.get(`/api/skill?${params.toString()}`);
      if (res.status === 200) {
        const newSkills = res.data as ISkill[];
        setSkills(newSkills);
      }
    } catch (err) {
      toastError(err);
    }
  };

  return (
    <div>
      <Accordion type="single" collapsible>
        <AccordionItem value="otherskills">
          <AccordionTrigger
            onClick={async () => {
              await fetchOtherSkills();
            }}
          >
            General Skills
          </AccordionTrigger>
          <AccordionContent>
            {!skills ? (
              <div>
                <LoaderCircle className="animate-spin" />
                <span className="sr-only">Loading...</span>
              </div>
            ) : (
              <ul className="flex flex-col gap-4">
                {skills.map((s) => {
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
