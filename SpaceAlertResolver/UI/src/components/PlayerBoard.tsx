import React from 'react';
import { PlayerColor } from '~/models';

export interface PlayerBoardProps {
	barcodeData: string[][];
	playerColor: PlayerColor;
}

export function PlayerBoard({ playerColor }: PlayerBoardProps) {
	console.log(playerColor);
	return <img src='\Boards\Green-1.png' />;
}
