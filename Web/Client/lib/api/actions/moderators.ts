"use server";
import {
	addModerator,
	deleteModerator,
	editModerator,
} from "@/lib/api/moderators";
import type { IModerator } from "@/lib/models/moderator/ModeratorModel";

export const actionAddModerator = async (user: IModerator) => {
	return await addModerator(user);
};

export const actionEditModerator = async (userId: string, user: IModerator) => {
	return await editModerator(userId, user);
};

export const actionDeleteModerator = async (userId: string) => {
	return await deleteModerator(userId);
};
