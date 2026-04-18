


type SuccessResponse<T> = {
    status: 200;
    data: T;
}

type ErrorResponse = {
    status: Exclude<number, 200>;
    message: string;
}

export type ApiResponse<T> = SuccessResponse<T> | ErrorResponse;


export async function getApiResponse<T>(response: Response): Promise<ApiResponse<T>> {
    if (response.status === 200) return {
        status: 200,
        data: await response.json()
    };
    return {
        status: response.status,
        message: await response.text()
    };
}