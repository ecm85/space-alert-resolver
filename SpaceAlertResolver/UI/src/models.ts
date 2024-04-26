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
	};
}

export interface ClientMessageReceivedEvent {
	event: 'ClientMessageReceived';
	data: {
		text: string;
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
