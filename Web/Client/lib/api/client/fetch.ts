import { NEXT_PUBLIC_API_URL } from "@/lib/env";

export const apiClientFetch = <T>(
	path: string,
	method: "GET" | "POST" | "PUT",
	body?: T,
) => {
	return fetch(`${NEXT_PUBLIC_API_URL}${path}`, {
		method: method,
		credentials: "include",
		headers: {
			"Content-Type": "application/json",
		},
		body: JSON.stringify(body),
	});
};

export const apiClientGET = async <T>(path: string) => {
	const res = await apiClientFetch(path, "GET");
	const data = await res.json();
	return data as T;
};

export const apiClientPOST = async <T, B>(path: string, body: B) => {
	const res = await apiClientFetch(path, "POST", body);
	const data = await res.json();
	return data as T;
};

export const apiClientPUT = async <T, B>(path: string, body: B) => {
	const res = await apiClientFetch(path, "PUT", body);
	const data = await res.json();
	return data as T;
};
