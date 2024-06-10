import { Injectable } from "@angular/core";
import { EndorsementService } from "@app/modules/submission/services/endorsement.service";
import { BehaviorSubject, Subscription, timer } from "rxjs";
import { tap } from "rxjs/operators";

export enum EndorsementProcessStatus {
	NOT_RUNNING,
	RUNNING,
	ERROR
}

@Injectable({
	providedIn: 'root'
})
export class EndorsementProcessMonitor {

	timerSub: Subscription;
	interval: number = 1.5;
	riskId: string;
	lastStatus: EndorsementProcessStatus;

	private _processStatus: BehaviorSubject<EndorsementProcessStatus> = new BehaviorSubject<EndorsementProcessStatus>(null);
	get processStatus$() { return this._processStatus.asObservable(); }

	constructor(
		private endorsementService: EndorsementService
	) {

	}

	start(riskId: string) {
		this.riskId = riskId;
		this.timerSub = timer(0, this.interval * 1000).pipe(
			tap(() => this.checkStatus().subscribe()),
		).subscribe();
	}

	stop() {
		this._processStatus.next(null);
		this.timerSub.unsubscribe();
	}

	private checkStatus() {
		return this.endorsementService.GetEndorsementProcessQueue(this.riskId).pipe(
			tap(data => {
				if (data != null) {
					if (data.isError) {
						this._processStatus.next(EndorsementProcessStatus.ERROR);
					} else {
						this._processStatus.next(EndorsementProcessStatus.RUNNING);
					}
				} else {
					this._processStatus.next(EndorsementProcessStatus.NOT_RUNNING);
				}

				this.lastStatus = this._processStatus.getValue();
			})
		);
	}
}
