"use client";
import type { NavigationEntry } from "@/components/navigation/entry";
import Link from "next/link";
import { usePathname } from "next/navigation";

export const NavEntry = ({ entry }: { entry: NavigationEntry }) => {
	const currentPath = usePathname();
	const active =
		(currentPath?.startsWith(entry.href ?? "") &&
			entry.href !== "/dashboard") ||
		(currentPath === "/dashboard" && entry.href === "/dashboard");

	const text = (
		<span
			className={`group flex items-center rounded-lg p-2 text-gray-900 ${
				active
					? "bg-gray-200 dark:bg-gray-600"
					: "dark:hover:bg-gray-700 hover:bg-gray-100"
			} dark:text-white select-none`}
		>
			<span className="mb-1 h-5 w-5 text-gray-500 transition duration-75 dark:group-hover:text-white dark:text-gray-400 group-hover:text-gray-900">
				{entry.icon}
			</span>
			<span className="ms-3">{entry.name}</span>
		</span>
	);

	if (entry.href) {
		return <Link href={entry.href}>{text}</Link>;
	}
	return text;
};
