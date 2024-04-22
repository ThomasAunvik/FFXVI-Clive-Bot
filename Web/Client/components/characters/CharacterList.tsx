import { replaceCDN } from "@/components/constants";
import { toastError } from "@/components/errors/ErrorHandler";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { NEXT_PUBLIC_API_URL } from "@/lib/env";
import type { ICharacter } from "@/lib/models/characters/CharacterModel";
import axios from "axios";
import { LoaderCircle } from "lucide-react";
import Image from "next/image";
import Link from "next/link";
import { useCallback, useEffect, useState } from "react";

export const CharacterList = () => {
	const [characters, setCharacters] = useState<ICharacter[] | null>(null);

	const fetchCharacters = useCallback(async () => {
		try {
			const res = await axios.get(`${NEXT_PUBLIC_API_URL}/api/character`);
			if (res.status === 200) {
				const newCharacters = res.data as ICharacter[];
				setCharacters(newCharacters);
			}
		} catch (err) {
			toastError(err);
		}
	}, []);

	useEffect(() => {
		fetchCharacters();
	}, [fetchCharacters]);

	return (
		<div>
			{characters === null ? (
				<div>
					<LoaderCircle className="animate-spin" />
					<span className="sr-only">Loading...</span>
				</div>
			) : (
				<div className="flex flex-row gap-4">
					{characters.map((s, i) => {
						const variant = s.variants?.find((v) => v.defaultVariant);

						return (
							<div key={`character-${s.id}`}>
								<Card style={{ width: "90vw", maxWidth: "18rem" }}>
									<CardHeader>
										<Image
											alt="character background"
											src="/static/images/features/char-bg.webp"
											width={500}
											height={300}
										/>
										<div>
											<Image
												alt="character preview"
												src={
													variant?.previewImageUrl
														? replaceCDN(variant?.previewImageUrl)
														: "https://cdn.discordapp.com/attachments/1075203421696700488/1075205728505167883/cliveRosfield_art_pc.png"
												}
												width={500}
												height={300}
											/>
										</div>
									</CardHeader>
									<CardContent className="z-[1]">
										<CardTitle>{s.name}</CardTitle>
										<p>
											{s.variants?.find((v) => v.defaultVariant)?.description ??
												"No Description"}
										</p>
										<Link href={`/dashboard/characters/${s.id}`}>
											<Button>Edit</Button>
										</Link>
									</CardContent>
								</Card>
							</div>
						);
					})}
				</div>
			)}
		</div>
	);
};
