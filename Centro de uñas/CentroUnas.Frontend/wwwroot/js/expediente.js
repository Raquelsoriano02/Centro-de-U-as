import { loadAll } from './api.js';
import { emptyState, formatDate, formatDateTime, fullName, h, money, openModal, toast } from './ui.js';

function dato(label, value) {
  return h('div', {}, h('dt', {}, label), h('dd', {}, value || '—'));
}

export async function openExpediente(cliente, { estadoBadge, saldoTurno }) {
  let data;
  try {
    data = await loadAll(['turnos', 'servicios', 'tecnicas', 'empleados', 'pagos']);
  } catch (error) {
    toast(error.message, 'error');
    return;
  }

  const turnos = data.turnos
    .filter(t => t.clienteId === cliente.id)
    .sort((a, b) => b.fechaHoraInicio.localeCompare(a.fechaHoraInicio));
  const vigentes = turnos.filter(t => t.estado !== 'Cancelado');
  const pagado = data.pagos
    .filter(p => turnos.some(t => t.id === p.turnoId))
    .reduce((suma, p) => suma + p.monto, 0);
  const pendiente = vigentes.reduce((suma, t) => suma + saldoTurno(data.pagos, t), 0);

  const historial = turnos.map(turno => {
    const servicio = data.servicios.find(s => s.id === turno.servicioId);
    const tecnica = data.tecnicas.find(t => t.id === turno.tecnicaId);
    const empleado = data.empleados.find(e => e.id === turno.empleadoId);
    const pagos = data.pagos.filter(p => p.turnoId === turno.id);
    return h('li', {},
      h('div', { class: 'timeline-head' },
        h('strong', {}, formatDateTime(turno.fechaHoraInicio)),
        estadoBadge(turno.estado)),
      h('div', { class: 'muted small' }, `${servicio?.nombre ?? '—'} con ${fullName(tecnica)} · atendió ${fullName(empleado)}`),
      h('div', { class: 'small' }, `Precio ${money(turno.precio)} · pagado ${money(pagos.reduce((s, p) => s + p.monto, 0))}${turno.estado !== 'Cancelado' ? ` · saldo ${money(saldoTurno(data.pagos, turno))}` : ''}`),
      turno.observacion ? h('div', { class: 'small muted' }, `Observación: ${turno.observacion}`) : null);
  });

  const modal = openModal({
    title: `Historial de ${fullName(cliente)}`,
    body: [
      h('dl', { class: 'info-grid' },
        dato('Teléfono', cliente.telefono),
        dato('Correo', cliente.email),
        dato('Cliente desde', formatDate(cliente.fechaRegistro)),
        dato('Turnos', `${vigentes.length} vigentes de ${turnos.length}`),
        dato('Total pagado', money(pagado)),
        dato('Saldo pendiente', money(pendiente))),
      h('h3', { class: 'section-title' }, `Historial de turnos (${turnos.length})`),
      turnos.length
        ? h('ul', { class: 'timeline' }, historial)
        : emptyState('calendar', 'Sin turnos todavía', 'Este cliente aún no tiene turnos registrados.'),
    ],
    footer: [h('button', { class: 'btn', type: 'button', onclick: () => modal.close() }, 'Cerrar')],
  });
}
