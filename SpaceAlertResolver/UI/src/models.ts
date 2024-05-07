export interface CreatedGameEvent {
	event: 'GameCreated';
	data: {
		code: string;
	};
}

export interface InvalidGameCodeEvent {
	event: 'InvalidGameCode';
}

export interface ExpiredGameCodeEvent {
	event: 'ExpiredGameCode';
}

export interface YouJoinedEvent {
	event: 'YouJoined';
}

export interface ClientJoinedEvent {
	event: 'ClientJoined';
	data: {
		name: string;
		connectionId: string;
	};
}

export interface ClientMessageReceivedEvent {
	event: 'ClientMessageReceived';
	data: {
		connectionId: string;
		barcodeData: string[][];
	};
}

export interface YourMessageSentEvent {
	event: 'YourMessageSent';
}

export type MessageEventData =
	| CreatedGameEvent
	| InvalidGameCodeEvent
	| ExpiredGameCodeEvent
	| YouJoinedEvent
	| ClientJoinedEvent
	| ClientMessageReceivedEvent
	| YourMessageSentEvent;

export interface Client {
	name: string;
	connectionId: string;
}
