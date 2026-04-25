


type SuccessResponse<T> = {
    success: true;
    status: number;
    data: T;
}

type ErrorResponse = {
    success: false;
    status: number;
    message: string;
}

export type ApiResponse<T> = SuccessResponse<T> | ErrorResponse;


export async function getApiResponse<T>(response: Response): Promise<ApiResponse<T>> {
    if (response.status >= 200 && response.status < 300) return {
        success: true,
        status: response.status,
        data: await response.json()
    };
    return {
        success: false,
        status: response.status,
        message: await response.text()
    };
}