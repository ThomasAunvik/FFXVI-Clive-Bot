import { apiDELETE, apiPOST, apiPUT } from "@/lib/api/fetch";
import type { IModerator } from "@/lib/models/moderator/ModeratorModel";
import "server-only";

export const addModerator = async (user: IModerator) => {
	return await apiPOST<IModerator[]>("/api/moderator", user);
};

export const editModerator = async (userId: string, user: IModerator) => {
	return await apiPUT<IModerator[]>(`/api/moderator/${userId}`, user);
};

export const deleteModerator = async (userId: string) => {
	return await apiDELETE<IModerator[]>(`/api/moderator/${userId}`);
};
