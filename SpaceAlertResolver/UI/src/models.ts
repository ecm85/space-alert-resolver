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
		barcodeData: string[];
		playerColor: PlayerColor;
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
	barcodeData?: string[];
	playerColor?: PlayerColor;
}

export enum PlayerColor {
	Red = 0,
	Blue,
	Green,
	Yellow,
	Purple,
}

export enum Orientation {
	Top,
	Bottom,
	Other,
}

export type ParsedBarcode = string | null;
