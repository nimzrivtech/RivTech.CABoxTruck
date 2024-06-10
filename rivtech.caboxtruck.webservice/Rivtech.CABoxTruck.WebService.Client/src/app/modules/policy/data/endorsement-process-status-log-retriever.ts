import { Injectable } from "@angular/core";
import { EndorsementService } from "@app/modules/submission/services/endorsement.service";
import { BehaviorSubject, Observable, Subscription, timer } from "rxjs";
import { switchMap, tap } from "rxjs/operators";
import { EndorsementProcessStatusLog } from "./endorsement-process-status-log";

@Injectable()
export class EndorsementProcessStatusLogRetriever {

	private _statusLogs = new BehaviorSubject<EndorsementProcessStatusLog[]>(null);
	get statusLogs$(): Observable<EndorsementProcessStatusLog[]> { return this._statusLogs.asObservable(); }

	statusLogTimerSubs: Subscription;
	interval: number = 1.5;
	riskId: string;
	running: boolean;

	constructor(
		private endorsementService: EndorsementService
	) {

	}

	start(riskId: string) {
		this.running = true;
		this.riskId = riskId;
		this._statusLogs.next(null);
		console.log('Started status log retriever.');

		this.endorsementService.clearStatusLogs$(this.riskId).subscribe(() => {
			this.statusLogTimerSubs = timer(0, this.interval * 1000).pipe(
				tap(() => this.retrieveLogs$().subscribe()),
				tap(() => console.log("Status log retrieved."))
			).subscribe();
		});
	}

	stop() {
		this.running = false;
		console.log(`Stopped status log retriever.`);
		this.retrieveLogs$().subscribe(); // one last retrieve log.
		this.statusLogTimerSubs?.unsubscribe();
	}

	clearLogs() {
		this._statusLogs.next([]);
	}

	private retrieveLogs$(): Observable<any> {
		console.log("Retrieving endorsement process status log...");
		return this.endorsementService.retrieveStatusLogs$(this.riskId).pipe(
			tap((logs: EndorsementProcessStatusLog[]) => {
				this._statusLogs.next(logs);
			})
		);
	}
}