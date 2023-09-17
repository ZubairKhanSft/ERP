import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { VendorModel, ContactModel } from '../../../models';

@Component({
    selector: 'app-vendor-contact-detail',
    templateUrl: './vendor.contact.detail.component.html'
})

export class VendorContactDetailComponent implements OnInit {
    @Input() model: VendorModel;
    @Output() moveForward = new EventEmitter();
    @Output() moveBackward = new EventEmitter();
    wizardStep = 4;

    constructor(private toastr: ToastrService) { }

    next() {
        this.moveForward.emit();
    }

    prev() {
        this.moveBackward.emit();
    }

    ngOnInit() {
        if (this.model.contacts.length > 0) {
            return;
        }
        const contact = new ContactModel();
        this.model.contacts.push(contact);
    }

    addContact() {
        let contact = this.model.contacts[this.model.contacts.length - 1];
        if (!contact.firstName
            && !contact.middleName
            && !contact.lastName
            && !contact.jobTitle
            && !contact.phone
            && !contact.email
            && !contact.address.streetNumber
            && !contact.address.streetName
            && !contact.address.postalCode
            && !contact.address.city) {
            this.toastr.error(`Please fill the detail of contact# ${this.model.contacts.length} before adding another`);
            return;
        }
        contact = new ContactModel();
        this.model.contacts.push(contact);
    }

    removeContact(index: number) {
        this.model.contacts.splice(index, 1);
    }
}
