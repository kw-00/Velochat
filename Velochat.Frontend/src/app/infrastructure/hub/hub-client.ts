import * as SignalR from "@microsoft/signalr";


type CommandParams = (null | boolean | number | string | object)[];

type RawCommandResultData = null | boolean | number | string | object;

type RawCommandResult = {
    status: number;
    message: string;
    hasData: boolean;
    data: RawCommandResultData;
}

type SuccessCommandResultWithData<T> = {
    status: number;
    success: true;
    message: string;
    hasData: true;
    data: T;
}

type SuccessCommandResultNoData = {
    status: number;
    success: true;
    message: string;
    hasData: false;
    data: null;
}

type ExceptionCommandResult = {
    status: number;
    success: false;
    message: string;
    hasData: false;
    data: null;
}

export type CommandResultWithData<T> 
    = SuccessCommandResultWithData<T> 
    | ExceptionCommandResult;

export type CommandResultNoData 
    = SuccessCommandResultNoData 
    | ExceptionCommandResult;


export class HubClient {
    private _connection: SignalR.HubConnection;

    constructor(connection: SignalR.HubConnection) {
        this._connection = connection;
    }

    public async invokeAsync<T>(
        dispatcher: string, 
        command: string, ...args: CommandParams
    ): Promise<CommandResultWithData<T>> {

        const result: RawCommandResult = await this._connection.invoke(dispatcher, command, ...args);
        let isSuccess: boolean = false;
        if (result.status >= 200 && result.status < 300) isSuccess = true;

        if (isSuccess) {
            return {
                status: result.status,
                success: true,
                message: result.message,
                hasData: true,
                data: result.data as T
            };
        } else {
            return {
                status: result.status,
                success: false,
                message: result.message,
                hasData: false,
                data: null
            };
        }
    }

    public async invokeNoContentAsync(
        dispatcher: string, 
        command: string, 
        ...args: CommandParams
    ) : Promise<CommandResultNoData> {

        const result: RawCommandResult = await this._connection.invoke(dispatcher, command, ...args);
        let isSuccess: boolean = false;
        if (result.status >= 200 && result.status < 300) isSuccess = true;

        if (isSuccess) {
            return {
                status: result.status,
                success: true,
                message: result.message,
                hasData: false,
                data: null
            };
        } else {
            return {
                status: result.status,
                success: false,
                message: result.message,
                hasData: false,
                data: null
            };
        }
    }
}