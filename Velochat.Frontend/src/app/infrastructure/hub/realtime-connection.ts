import * as SignalR from "@microsoft/signalr";


export class RealtimeConnection {
    private _connection: SignalR.HubConnection;

    private _connectedListeners = new Set<() => void>();

    constructor(connection: SignalR.HubConnection) {
        this._connection = connection;
    }

    /** Starts the connection.
     *
     * @returns {Promise<void>} A Promise that resolves when the connection 
     * has been successfully established, or rejects with an error.
     */
    async startAsync(): Promise<void> {
        return this._connection.start();
    }


    /** Stops the connection.
     *
     * @returns {Promise<void>} A Promise that resolves when the connection 
     * has been successfully terminated, or rejects with an error.
     */
    async stopAsync(): Promise<void> {
        return this._connection.stop();
    }


    /** Invokes a hub method on the server using the specified name and arguments.
     *
     * The Promise returned by this method resolves when the server 
     * indicates it has finished invoking the method. When the promise
     * resolves, the server has finished invoking the method. 
     * If the server method returns a result, it is produced as the result of
     * resolving the Promise.
     *
     * @typeparam T The expected return type.
     * @param {string} methodName The name of the server method to invoke.
     * @param {any[]} args The arguments used to invoke the server method.
     * @returns {Promise<T>} A Promise that resolves with the result 
     * of the server method (if any), or rejects with an error.
     */
    async invokeAsync<T = unknown>(methodName: string, ...args: unknown[]): Promise<T> {
        return this._connection.invoke(methodName, ...args);
    }


    /** Registers a handler that will be invoked when the 
     * hub method with the specified method name is invoked.
     *
     * @param {string} methodName The name of the hub method to define.
     * @param {Function} newMethod The handler that will be raised 
     * when the hub method is invoked.
     */
    on(methodName: string, newMethod: (...args: unknown[]) => unknown): void {
        this._connection.on(methodName, newMethod);
    }

    /** Removes the specified handler for the specified hub method.
     *
     * You must pass the exact same Function instance as was previously 
     * passed to {@link @microsoft/signalr.HubConnection.on}. 
     * Passing a different instance (even if the function
     * body is the same) will not remove the handler.
     *
     * @param {string} methodName The name of the method to remove handlers for.
     * @param {Function} method The handler to remove. This must be the same Function instance 
     * as the one passed to {@link @microsoft/signalr.HubConnection.on}.
     */

    off(
        methodName: string, 
        method?: (...args: unknown[]) => void
    ): void {
        if (method == undefined) {
            this._connection.off(methodName);
        } else {
            this._connection.off(methodName, method);
        }
    }


    /** Registers a handler that will be invoked when the connection 
     * starts or there is reconnection.
     *
     * @param {Function} callback The handler that will be invoked when 
     * the connection is closed. Optionally receives a single argument 
     * containing the error that caused the connection to close (if any).
     */
    onconnected(callback: (connectionId?: string) => void): void {
        this.onreconnected(callback);
        this._connectedListeners.add(callback);
    }

    /** Registers a handler that will be invoked when the connection is closed.
     *
     * @param {Function} callback The handler that will be invoked when 
     * the connection is closed. Optionally receives a single argument 
     * containing the error that caused the connection to close (if any).
     */
    onclose(callback: (error?: Error) => void): void {
        this._connection.onclose(callback);
    }


    /** Registers a handler that will be invoked when the connection starts reconnecting.
     *
     * @param {Function} callback The handler that will be invoked when the connection 
     * starts reconnecting. Optionally receives a single argument containing the error 
     * that caused the connection to start reconnecting (if any).
     */
    onreconnecting(callback: (error?: Error) => void): void {
        this._connection.onreconnecting(callback);
    }


    /** Registers a handler that will be invoked when the connection successfully reconnects.
     *
     * @param {Function} callback The handler that will be invoked when the connection successfully reconnects.
     */
    onreconnected(callback: (connectionId?: string) => void): void {
        this._connection.onreconnected(callback);
    }
}
