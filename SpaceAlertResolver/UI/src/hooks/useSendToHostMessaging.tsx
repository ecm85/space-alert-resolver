import { useCallback } from 'react';
import { PlayerInput } from '~/models';

export function useSendToHostMessaging() {
	const serialize = useCallback((playerInput: PlayerInput) => {
		return { message: JSON.stringify(playerInput), messageType: 'SendPlayerInput' };
	}, []);
	const deserialize = useCallback((message: string, messageType: string) => {
		switch (messageType) {
			case 'SendPlayerInput':
				return JSON.parse(message) as PlayerInput;
		}
		throw new Error(`Invalid message type ${messageType}.`);
	}, []);
	return {
		serialize,
		deserialize,
	};
}
